import React, { Component } from 'react';
import { Input, Label, FormGroup, Button } from 'reactstrap';
import { Formik, Field, Form, ErrorMessage } from 'formik';
import { fromJS } from 'immutable'
import { Link, Route } from 'react-router-dom'
import ObradaStavkeForm from './ObradaStavkeForm';
import ObradaNalogaPregledStavki from './ObradaNalogaPregledStavki';
import NaloziApi from '../NaloziApi'
import TipoviNalogaApi from '../../Common/TipoviNalogaApi'
import { FaPlus } from 'react-icons/fa';

class ObradaNalogaForm extends Component {

  constructor(props) {
    super(props);
    this.submitForm = this.submitForm.bind(this);
    this.validate = this.validate.bind(this);
    this.stornirajStavku = this.stornirajStavku.bind(this);
    this.ukloniStavku = this.ukloniStavku.bind(this);
    this.dodajStavku = this.dodajStavku.bind(this);
    this.state = {
      tipovi: [],
      initialValues: {
        Datum: new Date().toJSON().slice(0, 10),
        IdTip: '',
        Opis: '',
      },
      data: fromJS({
        Stavke: []
      })
    }
  }

  componentWillMount() {
    TipoviNalogaApi.getTipovi()
      .then(tipovi => this.setState({
        tipovi: tipovi
      }));

    this.getInitialValues();
  }

  componentDidMount() {
    this.props.setSubmitCallback(this.form.submitForm);
  }

  getInitialValues() {
    if (this.props.idNaloga) {
      NaloziApi.getPromena(this.props.idNaloga)
        .then(nalog => {
          nalog.Datum = nalog.Datum.slice(0, 10);
          this.setState({
            initialValues: nalog,
            data: this.state.data.set('Stavke', fromJS(nalog.Stavke))
          })
        });
    }
  }

  submitForm(values, actions) {
    const stavke = this.state.data.toJS().Stavke;
    values.Stavke = stavke;
    NaloziApi.submitNalog(values)
      .then(() => {
        actions.setSubmitting(false);
        this.props.close();
      },
        error => {
          actions.setSubmitting(false);
          actions.setStatus({ msg: error.message });
        }
      );
  }

  validate(values) {
    let errors = {};
    if (!values.Datum)
      errors.Datum = 'Datum naloga je obavezan podatak'
    if (!values.IdTip)
      errors.IdTip = 'Tip naloga je obavezan podatak'

    if (this.state.data.get('Stavke').isEmpty()) {
      this.form.setStatus({ msg: 'Nalog mora imati bar jednu stavku' });
      errors.Stavke = 'Nalog mora imati bar jednu stavku';
    }
    else {
      this.form.setStatus({ msg: '' });
    }
    return errors;
  }

  stornirajStavku(id) {
    let newState = this.state.data.updateIn(['Stavke'],
      list => list.update(
        list.findIndex(item => item.get('Id') === id),
        item => item.set('Stornirana', true)));
    this.setState({ data: newState }, () => this.validateForm());
  }

  ukloniStavku(id) {
    let newState = this.state.data.updateIn(['Stavke'],
      list => {
        const itemIdx = list.findIndex(item => item.get('Id') === id);
        return list.remove(itemIdx)
      });
    this.setState({ data: newState }, () => this.validateForm());
  }

  dodajStavku(stavka) {
    let newState = this.state.data.updateIn(['Stavke'],
      list => list.push(fromJS(stavka)));
    this.setState({ data: newState }, () => this.validateForm());
  }

  render() {
    const stavke = this.state.data.toJS().Stavke;
    return (
      <div>
        <Formik
          ref={(r) => this.form = r}
          enableReinitialize={true}
          initialValues={this.state.initialValues}
          validate={this.validate}
          onSubmit={(values, actions) => this.submitForm(values, actions)}
          render={({ values,
            errors,
            status,
            touched,
            handleBlur,
            handleChange,
            handleSubmit,
            isSubmitting,
            validateForm }) => {
            this.validateForm = validateForm;
            return <Form>
              <FormGroup>
                <Label >Datum naloga</Label>
                <Input
                  tag={Field}
                  name='Datum'
                  type='date'
                  component='input'
                />
                <ErrorMessage name="Datum" component="div" className="text-danger" />
              </FormGroup>
              <FormGroup>
                <Label >Tip naloga</Label>
                <Input
                  tag={Field}
                  name='IdTip'
                  component='select'>
                  <option></option>
                  {this.state.tipovi.map(t =>
                    <option key={t.Id} value={t.Id}>{t.Naziv}</option>)}
                </Input>
                <ErrorMessage name="IdTip" component="div" className="text-danger" />
              </FormGroup>
              <FormGroup>
                <Label >Opis</Label>
                <Input
                  tag={Field}
                  name='Opis'
                  type='text'
                  component='input'
                />
                <ErrorMessage name="Opis" component="div" className="text-danger" />
              </FormGroup>
              {status && status.msg && <div className="text-danger">{status.msg}</div>}
            </Form>
          }}
        />
        <Link to={`${this.props.url}/nova-stavka`}>
          <Button color="success" className='nova-stavka'><FaPlus />Dodaj stavku</Button>
        </Link>
        <ObradaNalogaPregledStavki stavke={stavke}
          ukloniStavku={this.ukloniStavku} stornirajStavku={this.stornirajStavku} />
        <Route exact path={`${this.props.url}/nova-stavka`}
          render={(props) => <ObradaStavkeForm {...props} dodajStavku={this.dodajStavku} />} />
      </div>
    );
  }
}

export default ObradaNalogaForm;

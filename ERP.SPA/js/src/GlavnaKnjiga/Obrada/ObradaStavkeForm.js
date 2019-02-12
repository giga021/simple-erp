import React, { Component } from 'react';
import { Formik, Field, Form, ErrorMessage } from 'formik';
import {
  Button, Modal, ModalHeader, ModalBody,
  ModalFooter, Input, Label, FormGroup
} from 'reactstrap';
import uuid from 'uuid/v4';
import KontaApi from 'Common/KontaApi'


class ObradaStavkeForm extends Component {

  constructor(props) {
    super(props);
    this.submitForm = this.submitForm.bind(this);
    this.validate = this.validate.bind(this);
    this.state = {
      konta: []
    }
    this.initialValues = {
      Id: uuid(),
      IdKonto: '',
      Konto: '',
      Duguje: '0',
      Potrazuje: '0',
      Opis: '',
      Stornirana: false,
    }
  }

  componentWillMount() {
    KontaApi.getKonta()
      .then(konta => this.setState({
        konta: konta
      }));
  }

  submitForm(values, actions) {
    values.Konto = this.state.konta.find(k => k.Id == values.IdKonto).Sifra
    this.props.dodajStavku(values);
    this.props.history.goBack();
  }

  validate(values) {
    let errors = {};

    if (!values.IdKonto)
      errors.IdKonto = 'Konto je obavezan podatak';

    if (isNaN(values.Duguje) || !values.Duguje)
      errors.Duguje = 'Iznos nije u korektnom formatu';
    if (isNaN(values.Potrazuje) || !values.Potrazuje)
      errors.Potrazuje = 'Iznos nije u korektnom formatu';

    if (!errors.Duguje && !errors.Potrazuje) {
      let duguje = parseFloat(values.Duguje);
      let potrazuje = parseFloat(values.Potrazuje);
      if (duguje !== 0 && potrazuje !== 0) {
        errors.Duguje = 'Iznos samo sa jedne strane stavke može biti definisan';
        errors.Potrazuje = 'Iznos samo sa jedne strane stavke može biti definisan';
      } else if (duguje === 0 && potrazuje === 0) {
        errors.Duguje = 'Iznos sa jedne strane stavke mora biti definisan';
        errors.Potrazuje = 'Iznos sa jedne strane stavke mora biti definisan';
      }
    }
    return errors;
  }

  render() {
    return <Modal isOpen={true}>
      <ModalHeader>Nova stavka</ModalHeader>
      <ModalBody>
        <Formik
          ref={(r) => this.form = r}
          initialValues={this.initialValues}
          validate={this.validate}
          handleSubmit={() => alert('handling')}
          onSubmit={(values, actions) => this.submitForm(values, actions)}
          render={({ values,
            errors,
            status,
            touched,
            handleBlur,
            handleChange,
            handleSubmit,
            isSubmitting }) => (
              <Form>
                <FormGroup>
                  <Label >Konto</Label>
                  <Input
                    tag={Field}
                    name='IdKonto'
                    component='select'>
                    <option></option>
                    {this.state.konta.map(t =>
                      <option key={t.Id} value={t.Id}>{t.Sifra} {t.Naziv}</option>)}
                  </Input>
                  <ErrorMessage name="IdKonto" component="div" className="text-danger" />
                </FormGroup>
                <FormGroup>
                  <Label >Duguje</Label>
                  <Input
                    tag={Field}
                    name='Duguje'
                    type='number'
                    component='input'
                  />
                  <ErrorMessage name="Duguje" component="div" className="text-danger" />
                </FormGroup>
                <FormGroup>
                  <Label >Potražuje</Label>
                  <Input
                    tag={Field}
                    name='Potrazuje'
                    type='number'
                    component='input'
                  />
                  <ErrorMessage name="Potrazuje" component="div" className="text-danger" />
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
            )}
        />
      </ModalBody>
      <ModalFooter>
        <Button onClick={() => this.form.submitForm()} color="primary">Sačuvaj</Button>
        <Button onClick={this.props.history.goBack}>Nazad</Button>
      </ModalFooter>
    </Modal>
  }
}

export default ObradaStavkeForm;

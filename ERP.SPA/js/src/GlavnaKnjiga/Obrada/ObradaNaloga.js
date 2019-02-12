import React, { Component } from 'react';
import {
  Button, Modal, ModalHeader, ModalBody,
  ModalFooter
} from 'reactstrap';

import ObradaNalogaForm from './ObradaNalogaForm';
import { FaSave } from 'react-icons/fa';

class ObradaNaloga extends Component {

  constructor(props) {
    super(props);
    this.setSubmitCallback = this.setSubmitCallback.bind(this);
    this.submitForm = this.submitForm.bind(this);
  }

  setSubmitCallback(submitForm) {
    this.submitFormCallback = submitForm;
  }

  submitForm() {
    this.submitFormCallback();
  }

  render() {
    return (
      <Modal isOpen={true}>
        <ModalHeader>
          {this.props.match.params.id ? 'Promena naloga' : 'Novi nalog'}
        </ModalHeader>
        <ModalBody>
          <ObradaNalogaForm
            idNaloga={this.props.match.params.id}
            setSubmitCallback={this.setSubmitCallback}
            url={this.props.match.url} 
            close={this.props.history.goBack}/>
        </ModalBody>
        <ModalFooter>
          <Button onClick={this.submitForm} color="primary" size="lg"><FaSave/>Sačuvaj</Button>
          <Button onClick={this.props.history.goBack} size="lg">Nazad</Button>
        </ModalFooter>
      </Modal>
    );
  }
}

export default ObradaNaloga;

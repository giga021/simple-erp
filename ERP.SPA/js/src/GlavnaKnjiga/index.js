import React, { Component } from 'react';
import { Route } from 'react-router-dom'
import ObradaNaloga from './Obrada/ObradaNaloga'
import Switch from 'react-router-dom/Switch';
import GlavnaKnjiga from './GlavnaKnjiga'
import NaloziApi from './NaloziApi';
import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import showMessage from 'showMessage';
import { connect } from 'react-redux';

class GlavnaKnjigaContainer extends Component {

  constructor(props) {
    super(props);
    this.zakljucajNalog = this.zakljucajNalog.bind(this);
    this.otkljucajNalog = this.otkljucajNalog.bind(this);
    this.obrisiNalog = this.obrisiNalog.bind(this);
    this.initSignalr = this.initSignalr.bind(this);
    this.startSignalr = this.startSignalr.bind(this);
    this.reloadData = this.reloadData.bind(this);
    this.state = {
      nalozi: []
    };
    this.reconnectSignalr = true;
  }

  componentWillMount() {
    this.reloadData();
  }

  componentDidMount() {
    this.initSignalr();
  }

  initSignalr() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl("glavna-knjiga-hub", { accessTokenFactory: () => this.props.access_token })
      //.withUrl("glavna-knjiga-hub")      
      .configureLogging(LogLevel.Trace)
      .build();
    this.hubConnection.on('data-changed', () => {
      this.reloadData();
    });
    this.hubConnection.on('knjizenje-error', (error) => {
      let valErrors = error.validationErrors &&
        error.validationErrors.map(e => ` - ${e.message}`).join('\r\n');
      showMessage.error(error.message, 'Greška pri knjiženju', valErrors)
    });
    this.hubConnection.onclose(() => {
      if (this.reconnectSignalr) {
        this.startSignalr();
      }
    });
    this.startSignalr();
  }

  startSignalr() {
    this.hubConnection.start()
      .catch(err => {
        if(err.message)
          showMessage.error(`SignalR: ${err.message}`);
        setTimeout(() => this.startSignalr(), 10000);
      });
  }

  componentWillUnmount() {
    console.log('Stopping SignalR...')
    this.reconnectSignalr = false;
    this.hubConnection.stop();
  }

  reloadData() {
    NaloziApi.getGlavnaKnjiga()
      .then(nalozi => this.setState({ nalozi: nalozi }));
  }

  zakljucajNalog(id, version) {
    NaloziApi.zakljucaj(id, version);
  }

  otkljucajNalog(id, version) {
    NaloziApi.otkljucaj(id, version);
  }

  obrisiNalog(id, version) {
    NaloziApi.obrisi(id, version);
  }

  render() {
    return (
      <div>
        <GlavnaKnjiga zakljucajNalog={this.zakljucajNalog}
          otkljucajNalog={this.otkljucajNalog}
          obrisiNalog={this.obrisiNalog}
          nalozi={this.state.nalozi}
          url={this.props.match.url} />
        <Switch>
          <Route path={`${this.props.match.url}/novi-nalog`} component={ObradaNaloga} />
          <Route path={`${this.props.match.url}/:id`} component={ObradaNaloga} />
        </Switch>
      </div>
    );
  }
}
function mapStateToProps(state) {
  return {
    access_token: state.oidc.user.access_token
  };
}
function mapDispatchToProps(dispatch) {
  return {
    dispatch
  };
}
export default connect(mapStateToProps, mapDispatchToProps)(GlavnaKnjigaContainer);
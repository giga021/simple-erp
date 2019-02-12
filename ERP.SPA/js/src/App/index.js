import React, { Component } from 'react';
import { Provider } from 'react-redux';
import { OidcProvider } from 'redux-oidc';
import Header from './Header'
import Main from './Main'
import Entry from './Entry'
import store from 'store';
import userManager from 'userManager';
import { Switch, Route } from 'react-router-dom'
import CallbackPage from '../CallbackPage'

import './bootstrap.min-cosmo.css';
import 'react-toastify/dist/ReactToastify.min.css';

class App extends Component {

  render() {
    return (
      <Provider store={store}>
        <OidcProvider store={store} userManager={userManager}>
          <div>
            <Entry />
            <Route exact path='/callback' component={CallbackPage} />
          </div>
        </OidcProvider>
      </Provider>
    );
  }
}

export default App;

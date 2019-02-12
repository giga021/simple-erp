import React, { Component } from 'react';
import { connect } from 'react-redux';
import Header from './Header'
import Main from './Main'
import userManager from 'userManager';
import LoginPage from './LoginPage';
import { withRouter } from 'react-router';

class Entry extends Component {

  render() {
    let user = this.props.user;

    if (!user || user.expired) {
      return <LoginPage />
    }
    else {
      return <div>
        <Header />
        <Main />
      </div>
    }
  }
}

function mapStateToProps(state) {
  return {
    user: state.oidc.user
  };
}

function mapDispatchToProps(dispatch) {
  return {
    dispatch
  };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(Entry));

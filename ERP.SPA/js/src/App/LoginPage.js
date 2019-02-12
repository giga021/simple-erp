import React from "react";
import userManager from "userManager";

class LoginPage extends React.Component {

  login(event) {
    userManager.signinRedirect();
  }

  render() {
    return (
      <div style={styles.root}>
        <h3>Simple ERP demo aplikacija</h3>
        <p>Prijavite se za nastavak</p>
        <button onClick={this.login}>Prijava</button>
      </div>
    );
  }
}

const styles = {
  root: {
    display: "flex",
    flexDirection: "column",
    justifyContent: "space-around",
    alignItems: "center",
    flexShrink: 1
  }
};

export default LoginPage;

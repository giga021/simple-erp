import React from "react";
import { connect } from "react-redux";
import { CallbackComponent } from "redux-oidc";
import userManager from "userManager";

class CallbackPage extends React.Component {
    render() {
        return (
            <CallbackComponent
                userManager={userManager}
                successCallback={() => {
                    console.log('success');
                    this.props.history.push("/");
                }}
                errorCallback={error => {
                    console.error(error);
                    this.props.history.push("/");
                }}>
                <div>Redirecting...</div>
            </CallbackComponent>
        );
    }
}

export default connect()(CallbackPage);
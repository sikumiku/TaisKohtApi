import React, { Component } from 'react';
import AuthService from './AuthService';

class Logout extends Component {
    constructor() {
        super();
        this.Auth = new AuthService();
    }

    componentDidMount() {
        this.Auth.logout()
        this.props.history.replace('/');
    }

    render() {
        return (
               <p> Logging out...</p>
        );
    }
}

export default Logout;
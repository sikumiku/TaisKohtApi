import React, { Component } from 'react';
import { Button, FormGroup, FormControl, ControlLabel } from "react-bootstrap";
import "./Login.css";

class LoginForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            email: "",
            password: ""
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    validateForm() {
        return this.state.email.length > 0 && this.state.password.length > 0;
    }

    handleChange(event) {
        this.setState({
             [event.target.id]: event.target.value
        });
    }

    handleSubmit(event) {
        alert('Login was submitted: ' + this.state.email + ' with password ' + this.state.password + '.');
        event.preventDefault();
        fetch("http://localhost:64376/api/account/login",
                {
                    method: "POST",
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(this.state)
                })
            .then(function (response) {
                return response.json();
            })
            .then(function (data) {
                alert('Received JWT token: ' + data.token);
                sessionStorage.setItem('jwtToken', data.token);
            });
    }

    render() {
        return (
            <div className="Login">
                <p>Ei ole veel kontot loonud? Registreeru 
                <a href="/register">siin.</a></p>
                <form onSubmit={this.handleSubmit}>
                    <FormGroup controlId="email" bsSize="large">
                        <ControlLabel>E-mail</ControlLabel>
                        <FormControl
                            autoFocus
                            type="email"
                            value={this.state.email}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="password" bsSize="large">
                        <ControlLabel>Salasõna</ControlLabel>
                        <FormControl
                            value={this.state.password}
                            onChange={this.handleChange}
                            type="password"
                        />
                    </FormGroup>
                    <Button
                        block
                        bsSize="large"
                        disabled={!this.validateForm()}
                        type="submit"
                            >
                        SISENE
                    </Button>
                </form>
            </div>
    );
    }
}


export default LoginForm;
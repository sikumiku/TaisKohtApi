import React, { Component } from 'react';
import './Login.css';
import { Link } from 'react-router-dom';
import AuthService from './AuthService';
import { Button, FormGroup, FormControl, ControlLabel } from "react-bootstrap";

class Login extends Component {
    constructor() {
        super();
        this.handleChange = this.handleChange.bind(this);
        this.handleFormSubmit = this.handleFormSubmit.bind(this);
        this.Auth = new AuthService();
        this.state = {
            //Aa12345678.
            email: "",
            password: ""
        };
    }
    componentWillMount() {
        if (this.Auth.loggedIn())
            this.props.history.replace('/');
    }
    render() {
        return (
            <div className="center">
                <div>
                   Do not have an account?<br/> 
                    <Link to="/register">
                        Register here<br/>
                    </Link>
                </div>
                    <h1>Login</h1>
                    <div className="Login">
                        <form onSubmit={this.handleFormSubmit}>
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
                                <ControlLabel>Password</ControlLabel>
                                <FormControl
                                    value={this.state.password}
                                    onChange={this.handleChange}
                                    type="password"
                                />
                            </FormGroup>
                        <Button block bsSize="large" type="submit">Login</Button>
                        </form>
                  </div>
            </div>
        );
    }

    handleFormSubmit(e) {
        e.preventDefault();

        this.Auth.login(this.state.email, this.state.password)
            .then(res => {
                this.props.history.replace('/');
            })
            .catch(err => {
                err.response.json().then(msg => {
                    alert(msg);
                });
            });
    }

    handleChange(e) {
        this.setState(
            {
                [e.target.id]: e.target.value
            }
        )
    }
}

export default Login;
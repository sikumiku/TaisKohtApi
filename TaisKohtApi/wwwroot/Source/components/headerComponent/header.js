import React, { Component } from 'react';
import { NavItem, Navbar } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import AuthService from '../pages/auth/AuthService';
import withAuth from '../pages/auth/withAuth';
const Auth = new AuthService();

class Header extends Component {

    handleLogout() {
        Auth.logout()
        this.props.history.replace('/');
    }

    render() {
        let authButton;
        if (Auth.loggedIn()) {
            console.log('Is logged in.');
            authButton = 
                <LinkContainer to="/logout">
                    <NavItem eventKey={1}>Logout</NavItem>
                </LinkContainer>;
        } else {
            console.log('Is logged out.');
            authButton = 
                <LinkContainer to="/login">
                    <NavItem eventKey={1}>Login</NavItem>
                </LinkContainer>;
        }

        let userName; 
        if (Auth.loggedIn()) {
            console.log(Auth.getProfile());
            userName = <LinkContainer to="/profile">
                            <NavItem eventKey={1}>Profile : {Auth.getProfile().sub} </NavItem>
                        </LinkContainer>
        } else {
            userName = "";
        }

        return (
            <header>
                <nav className="navbar navbar-inverse">
                    <Navbar.Header>
                        <Navbar.Brand>
                            <a className="navbar-brand" href="#brand">Täis Kõht</a>
                        </Navbar.Brand>
                        <Navbar.Toggle />
                    </Navbar.Header>
                    <Navbar.Collapse>
                        <ul className="nav navbar-nav navbar-right">r
                            <LinkContainer to="/">
                                <NavItem eventKey={1}>Daily Specials</NavItem>
                            </LinkContainer>
                            <LinkContainer to="/restaurants">
                                <NavItem eventKey={1}>Restaurants</NavItem>
                            </LinkContainer>
                            {authButton}

                            {userName}
                        </ul>
                    </Navbar.Collapse>
                </nav>
            </header>
        );
    }
}

export default Header;

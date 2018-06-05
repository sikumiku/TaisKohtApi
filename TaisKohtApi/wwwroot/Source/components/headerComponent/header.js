import React, { Component } from 'react';
import { NavItem, Navbar, NavDropdown, MenuItem } from 'react-bootstrap';
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
        let authContent; 
        if (Auth.loggedIn()) {
            console.log(Auth.getProfile());
            authContent =
                <NavDropdown eventKey={3} title="Profile settings" id="basic-nav-dropdown">
                    <MenuItem eventKey={3.1}>
                        <LinkContainer to="/profile/restaurants">
                            <NavItem eventKey={1}>Restaurants</NavItem>
                        </LinkContainer>
                    </MenuItem>
                    <MenuItem eventKey={3.1}>
                        <LinkContainer to="/profile/dishes">
                            <NavItem eventKey={1}>Dishes</NavItem>
                        </LinkContainer>
                    </MenuItem>
                    <MenuItem eventKey={3.1}>
                        <LinkContainer to="/profile/menus">
                            <NavItem eventKey={1}>Menus</NavItem>
                        </LinkContainer>
                    </MenuItem>
                    <MenuItem divider />
                    <MenuItem eventKey={3.3}>
                        <LinkContainer to="/logout">
                            <NavItem>Logout {Auth.getProfile().sub}</NavItem>
                        </LinkContainer>
                    </MenuItem>
                </NavDropdown>;
        } else {
            authContent =
                <LinkContainer to="/login">
                    <NavItem eventKey={1}>Login</NavItem>
                </LinkContainer>;
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
                             {authContent}
                        </ul>
                    </Navbar.Collapse>
                </nav>
            </header>
        );
    }
}

export default Header;

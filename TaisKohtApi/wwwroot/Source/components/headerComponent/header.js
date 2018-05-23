import React, { Component } from 'react';
import { NavItem, Navbar } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';


class Header extends Component {
    render() {
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
                                <NavItem eventKey={1}>PÄEVAPAKKUMISED</NavItem>
                            </LinkContainer>
                            <LinkContainer to="/restaurants">
                                <NavItem eventKey={1}>RESTORANID</NavItem>
                            </LinkContainer>
                            <LinkContainer to="/login">
                                <NavItem eventKey={1}>SISENE</NavItem>
                            </LinkContainer>
                        </ul>
                    </Navbar.Collapse>
                </nav>
            </header>
        );
    }
}

export default Header;

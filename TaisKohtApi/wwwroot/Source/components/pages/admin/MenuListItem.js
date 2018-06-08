import React, { Component } from 'react';
import { Link, Route } from 'react-router-dom';
import { ReactCSSTransitionGroup } from 'react-addons-css-transition-group';

export default class MenuListItem extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            menu: this.props.menu
        };
    }
    render() {

        return (
        <div class="panel panel-default">
            <div class="panel-heading">
            <h3 class="panel-title"><a href="#" onClick={() => { this.getMenuInfo() }}>{this.state.menu.name}</a></h3>
    </div>

    </div>

);
}
}

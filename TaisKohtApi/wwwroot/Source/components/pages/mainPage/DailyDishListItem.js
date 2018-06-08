import React, { Component } from 'react';
import { Link, Route } from 'react-router-dom';
import { ReactCSSTransitionGroup } from 'react-addons-css-transition-group';

export default class DailyDishListItem extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            dailyDish: this.props.dailyDish
        };
    }
    render() {

        return (
        <div class="panel panel-default">
            <div class="panel-heading">
                    <h3 class="panel-title"><a href="#" onClick={() => { this.getDailyDishInfo() }}>{this.state.dailyDish.title}: {this.state.dailyDish.description} Price: {this.state.dailyDish.price}</a></h3>
    </div>

    </div>

);
}
}

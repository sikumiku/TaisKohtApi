import React, { Component } from 'react';
import { Link , Route} from 'react-router-dom';
import RestaurantMenu from '../menu/RestaurantMenu';
import { ReactCSSTransitionGroup } from 'react-addons-css-transition-group';
import Dish from '../menu/Dish';

export default class RestaurantListItem extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            restaurant: this.props.restaurant
        };
    }
    render() {
           
        return (
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title"><a href="#" onClick={() => { this.getRestaurantInfo() }}>{this.state.restaurant.name} {this.state.restaurant.url} {this.state.restaurant.contactNumber} {this.state.restaurant.email}</a></h3>
                </div>
             
            </div>

        );
    }
}

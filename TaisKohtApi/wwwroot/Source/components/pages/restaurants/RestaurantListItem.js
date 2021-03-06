﻿import React, { Component } from 'react';
import { Link , Route} from 'react-router-dom';
import RestaurantMenu from '../menu/RestaurantMenu';
import { ReactCSSTransitionGroup } from 'react-addons-css-transition-group';
import Dish from '../menu/Dish';

export default class RestaurantListItem extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            restaurantInfo: null,
            spinner: 'EMPTY',
            restaurant: this.props.restaurant
        };
    }
    render() {
            let contents;
            if (this.state.spinner === 'EMPTY') {
                contents = "";
            } else if (this.state.spinner === 'LOADING') {
                contents = "spinner";
            } else if (this.state.spinner === 'LOADED' && this.state.restaurantInfo != null) {
                contents = RestaurantListItem.renderExpandedInfo(this.state.restaurantInfo);
            }
       
        return (
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title"><a href="#" onClick={() => { this.getRestaurantInfo() }}>{this.state.restaurant.name} {this.state.restaurant.url} {this.state.restaurant.contactNumber} {this.state.restaurant.email}</a></h3>
                </div>
                   {contents}  
            </div>

        );
    }

    getRestaurantInfo() {
        this.state.spinner = 'LOADING';
        fetch('/api/v1/Restaurants/' + this.state.restaurant.restaurantId)
            .then(response => response.json())
            .then(data => {
                this.setState({ restaurantInfo: data, spinner: 'LOADED' });
            });
    }

    static renderExpandedInfo(restaurantInfo) {
        console.log(restaurantInfo);
        return <div class="panel-body">
                    {restaurantInfo.menus.map(menu =>
                        <RestaurantMenu menu={menu}/>
                    )}
                {restaurantInfo.dishes.map(dish =>
                    <Dish dish={dish}/>
                )}
        </div>;
    }
}

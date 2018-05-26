import React, { Component } from 'react';
import { Link , Route} from 'react-router-dom';
import RestaurantMenu from '../menu/RestaurantMenu';

class RestaurantListItem extends Component {
    render() {
        const restaurant = this.props.restaurant;
        return (
            <div>
                {restaurant.name}
                {restaurant.url}
                {restaurant.contactNumber}
                {restaurant.email}
                <div>
                    <Link to={'/restaurant/' + restaurant.restaurantId}>
                        Menu
                    </Link>
                </div>
            </div>

        );
    }
}

export default RestaurantListItem;
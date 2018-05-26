import React, { Component } from 'react';
import { LinkContainer } from 'react-router-bootstrap';

class MenuItem extends Component {
    render() {
        const dish = this.props.dish;
        return (
            <div>
                {dish.name}
                {restaurant.url}
                {restaurant.contactNumber}
                {restaurant.email}
                <div>
                    <LinkContainer to="/restaurant/{restaurant.id}">
                        Menüü
                    </LinkContainer>
                </div>
            </div>
        );
    }
}

export default MenuItem;
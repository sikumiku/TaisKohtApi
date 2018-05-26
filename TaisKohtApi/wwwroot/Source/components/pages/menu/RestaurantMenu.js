import * as React from 'react';
import 'es6-promise';
import 'isomorphic-fetch';
import MenuItem from './MenuItem';
export default class RestaurantMenu extends React.Component {
    constructor() {
        super();
        this.state = { menu: [], loading: true };
        this.restaurantId = this.props.restaurantId;
        this.refreshData();
    }
    render() {
        let contents = this.state.loading ? <p><em>Loading...</em></p>
            : RestaurantMenu.renderMenuList(this.state.menu);

        return <div>
            <h1>Restaurant Menu</h1>
            <button onClick={() => { this.refreshData() }}>Refresh</button>
            <p>This component fetches menu data from the server.</p>
            {contents}
        </div>;
    }
    refreshData() {
        fetch('api/v1/restaurant/' + this.restaurantId)
            .then(response => response.json())
            .then(data => {
                this.setState({ menu: data, loading: false });
            });
    }
    static renderMenuList(menu) {
        return <div className='menuItem'>
            {menu.map(dish =>
                <MenuItem dish={dish} />
            )}
        </div>;
    }
}
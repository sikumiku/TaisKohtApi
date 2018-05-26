import * as React from 'react';
import 'es6-promise';
import 'isomorphic-fetch';
import RestaurantListItem from './RestaurantListItem';
export default class RestaurantList extends React.Component {
    constructor() {
        super();
        this.state = { restaurants: [], loading: true };
        this.refreshData();
    }
    render() {
        let contents = this.state.loading ? <p><em>Loading...</em></p>
            : RestaurantList.renderRestaurantTable(this.state.restaurants);

        return <div>
            <h1>Restaurant List</h1>
            <button onClick={() => { this.refreshData() }}>Refresh</button>
            <p>This component fetches restaurant data from the server.</p>
            {contents}
        </div>;
    }
    refreshData() {
        fetch('api/v1/Restaurants')
            .then(response => response.json())
            .then(data => {
                console.log(data);
                this.setState({ restaurants: data, loading: false });
            });
    }
    static renderRestaurantTable(restaurants) {
        return <div className='restaurantList'>
            {restaurants.map(restaurant =>
                <RestaurantListItem restaurant={restaurant} />
            )}
        </div>;
    }
}
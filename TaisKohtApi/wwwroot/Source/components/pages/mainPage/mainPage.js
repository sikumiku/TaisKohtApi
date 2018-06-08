import React, { Component } from 'react';
import DailyDishListItem from './DailyDishListItem';

class MainPage extends Component {
    constructor() {
        super();
        this.state = { dailyDishes: [], loading: true };
        this.refreshData();
    }
    render() {
        let contents = this.state.loading ? <p><em>Loading...</em></p>
            : MainPage.renderDishesTable(this.state.dailyDishes);

        return <div>
            <div className="page-header">Daily Specials</div>
            {contents}
            </div>;
    }
    refreshData() {
        fetch('api/v1/dishes/daily')
            .then(response => response.json())
            .then(data => {
                console.log(data);
                this.setState({ dailyDishes: data, loading: false });
            });
    }
    static renderDishesTable(dailyDishes) {
        return <div className='restaurantList'>
            {dailyDishes.map(dailyDish =>
            <DailyDishListItem dailyDish={dailyDish} />
    )}
</div>;
}
}

export default MainPage;

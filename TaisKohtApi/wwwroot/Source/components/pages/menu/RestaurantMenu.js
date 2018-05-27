import * as React from 'react';
import 'es6-promise';
import 'isomorphic-fetch';
import Dish from './Dish';

export default class RestaurantMenu extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            menuContent: null,
            spinner: 'EMPTY',
            menu: this.props.menu
        };
    }
    render() {
        let contents;
        if (this.state.spinner === 'EMPTY') {
            contents = "";
        } else if (this.state.spinner === 'LOADING') {
            contents = "spinner";
        } else if (this.state.spinner === 'LOADED' && this.state.menuContent != null) {
            contents = RestaurantMenu.renderExpandedInfo(this.state.menuContent);
        }

        return (
            <div>
                <b>
                    <a href="#" onClick={() => { this.getMenuContent() }}>{this.state.menu.name}</a></b>
                {contents}
            </div>

        );
    }

    getMenuContent() {
        this.state.spinner = 'LOADING';
        //this.state.menu.menuId
        fetch('/api/v1/Menus/5')
            .then(response => response.json())
            .then(data => {
                this.setState({ menuContent: data, spinner: 'LOADED' });
            });
    }

    static renderExpandedInfo(menuContent) {
        console.log(menuContent);
        return <div>
            <b>Dishes :</b>
            {menuContent.dishes.map(dish =>
                <Dish dish={dish} />
            )}
        </div>;
    }
}
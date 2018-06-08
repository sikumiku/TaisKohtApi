import * as React from 'react';
import 'es6-promise';
import 'isomorphic-fetch';

export default class Dish extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            dishData: null,
            spinner: 'EMPTY',
            dish: this.props.dish
        };
    }
    render() {
        let contents;
        if (this.state.spinner === 'EMPTY') {
            contents = "";
        } else if (this.state.spinner === 'LOADING') {
            contents = "spinner";
        } else if (this.state.spinner === 'LOADED' && this.state.dishData != null) {
            contents = Dish.renderDishDetails(this.state.dishData);
        }

        return (
            <div>
                <b>
                    <a href="#" onClick={() => { this.getDishDetails() }}>{this.state.dish.title}</a> {this.state.dish.price} </b>
                {contents}
            </div>

        );
    }

    getDishDetails() {
        this.state.spinner = 'LOADING';
        fetch('/api/v1/Dishes/' + this.state.dish.dishId)
            .then(response => response.json())
            .then(data => {
                this.setState({ dishData: data, spinner: 'LOADED' });
            });
    }

    static renderDishDetails(dishDetails) {
        console.log('renderDishDetails');
        console.log(dishDetails);
        return <div>
            <b>Dish Info :</b>
            <div> {dishDetails.description} </div>
            <div> Gluten free : {dishDetails.glutenFree} </div>
            
            <b>Dish Ingredients : </b>
            {dishDetails.ingredients.map(ingridient =>
                <div>
                    <div> {ingredient.name} </div>
                    <div> {ingredient.amount} </div>
                    <div> {ingredient.amountUnit} </div>
                </div>
            )}
        </div>;
    }
}
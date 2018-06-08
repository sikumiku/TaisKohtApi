import * as React from 'react';
import 'es6-promise';
import 'isomorphic-fetch';
import { Button, FormGroup, FormControl, ControlLabel } from "react-bootstrap";
import AuthService from '../Auth/AuthService';
import Dish from './Dish';
const Auth = new AuthService();
import './admin.css';
import axios from 'axios';


export default class AdminDishes extends React.Component {
    constructor() {
        super();
        ///     {
        ///         "Title" : "Chicken Kiev",
        ///         "Description" : "Tasty meal",
        ///         "AvailableFrom" : "2018-05-27T20:51:22.508Z",
        ///         "AvailableTo" : "2018-05-27T20:51:22.508Z",
        ///         "ServeTime" : "2018-05-27T20:51:22.508Z",
        ///         "Vegan" : false,
        ///         "LactoseFree" : false,
        ///         "GlutenFree" : false,
        ///         "Kcal" : 400.00,
        ///         "WeightG" : 300.00,
        ///         "Price" : 7.25,
        ///         "DailyPrice" : 5.00,
        ///         "Daily" : true,
        ///         "RestaurantId" : 1,
        ///         "MenuId" : 1,
        ///         "PromotionId" : 1
        ///     }

        this.state = {
            userDishes: [],
            userRestaurants: [],
            userMenus: [],
            loading: true,

            title: null,
            Description: null,
            AvailableFrom: null,
            AvailableTo: null,
            ServeTime: null,
            Vegan: null,
            LactoseFree: null,
            GlutenFree: null,
            Kcal: null,
            WeightG: null,
            Price: null,
            DailyPrice: null,
            Daily: null,
            RestaurantId: null,
            MenuId: null,
        };

        this.handleChange = this.handleChange.bind(this);
        this.postMenu = this.postDish.bind(this);

        this.getUserMenus();
        this.getUserRestaurants();
        this.getUserDishes();
    }
    render() {
        let userDishes = this.state.loading ? <p><em>Loading...</em></p>
            : AdminDishes.renderUserDishList(this.state.userDishes);

        let menuOptionValues =
            this.state.userMenus.map(menu =>
                <option value={menu.menuId}>{menu.name}</option>
            );

        let restaurantOptionValues = this.state.userRestaurants.map(restaurant =>
            <option value={restaurant.restaurantId}>{restaurant.name}</option>
        );

        return <div>
            <div className="page-header">Your dishes</div>
            {userDishes}

            <div className="EditForm">
                <div className="page-header">Add new Dish</div>
                <form onSubmit={this.postDish}>
                    <FormGroup controlId="title" bsSize="small">
                        <ControlLabel>Dish title</ControlLabel>
                        <FormControl
                            autoFocus
                            type="text"
                            value={this.state.title}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="description" bsSize="small">
                        <ControlLabel>Dish description</ControlLabel>
                        <FormControl
                            autoFocus
                            type="text"
                            value={this.state.description}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="AvailableFrom" bsSize="small">
                        <ControlLabel>Available from</ControlLabel>
                        <FormControl
                            autoFocus
                            type="date"
                            value={this.state.AvailableFrom}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="AvailableTo" bsSize="small">
                        <ControlLabel>Available To</ControlLabel>
                        <FormControl
                            autoFocus
                            type="date"
                            value={this.state.AvailableTo}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="ServeTime" bsSize="small">
                        <ControlLabel>Serve Time</ControlLabel>
                        <FormControl
                            autoFocus
                            type="date"
                            value={this.state.ServeTime}
                            onChange={this.handleChange}
                        />
                    </FormGroup>

                    <FormGroup controlId="Vegan" bsSize="small">
                        <Checkbox value={this.state.Vegan}>Vegan</Checkbox>
                    </FormGroup>

                    <FormGroup controlId="LactoseFree" bsSize="small">
                        <Checkbox value={this.state.LactoseFree}>Lactose Free</Checkbox>
                    </FormGroup>

                    <FormGroup controlId="GlutenFree" bsSize="small">
                        <Checkbox value={this.state.GlutenFree}>Gluten Free</Checkbox>
                    </FormGroup>

                    <FormGroup controlId="Kcal" bsSize="small">
                        <ControlLabel>Kcal</ControlLabel>
                        <FormControl
                            autoFocus
                            type="number"
                            value={this.state.Kcal}
                            onChange={this.handleChange}
                        />
                    </FormGroup>

                    <FormGroup controlId="WeightG" bsSize="small">
                        <ControlLabel>Weight (G)</ControlLabel>
                        <FormControl
                            autoFocus
                            type="number"
                            value={this.state.WeightG}
                            onChange={this.handleChange}
                        />
                    </FormGroup>

                    <FormGroup controlId="Price" bsSize="small">
                        <ControlLabel>Price</ControlLabel>
                        <FormControl
                            autoFocus
                            type="number"
                            value={this.state.Price}
                            onChange={this.handleChange}
                        />
                    </FormGroup>

                    <FormGroup controlId="DailyPrice" bsSize="small">
                        <ControlLabel>DailyPrice</ControlLabel>
                        <FormControl
                            autoFocus
                            type="number"
                            value={this.state.Price}
                            onChange={this.handleChange}
                        />
                    </FormGroup>

                    <FormGroup controlId="menuId" bsSize="small">
                        <ControlLabel>Menu</ControlLabel>
                        <FormControl componentClass="select"
                            placeholder="select"
                            value={this.state.MenuId}
                            onChange={this.handleChange}>
                            {menuOptionValues}
                        </FormControl>
                    </FormGroup>

                    <FormGroup controlId="restaurantId" bsSize="small">
                        <ControlLabel>Restaurant</ControlLabel>
                        <FormControl componentClass="select"
                            placeholder="select"
                            value={this.state.restaurantId}
                            onChange={this.handleChange}>
                            {restaurantOptionValues}
                        </FormControl>
                    </FormGroup>
                    <Button block bsSize="large" type="submit">Add Menu</Button>
                </form>
            </div>
        </div>;
    }

    postDish(e) {
        e.preventDefault();
        let postData = JSON.stringify({
            'title': this.state.title,
            'Description': this.state.Description,
            'AvailableFrom': this.state.AvailableFrom,
            'AvailableTo': this.state.AvailableTo,
            'ServeTime': this.state.ServeTime,
            'Vegan': this.state.Vegan,
            'LactoseFree': this.state.LactoseFree,
            'GlutenFree': this.state.GlutenFree,
            'Kcal': this.state.Kcal,
            'WeightG': this.state.WeightG,
            'Price': this.state.Price,
            'DailyPrice': this.state.DailyPrice,
            'Daily': this.state.Daily,
            'RestaurantId': this.state.RestaurantId,
            'MenuId': this.state.MenuId
        });

        let headers = {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + Auth.getToken(),
            }
        };

        axios.post('/api/v1/Dishes', postData, headers)
            .then(response => {
                console.log(data);
                this.getUserDishes();
            })
            .catch(err => {
                console.log(err.response.data);
                alert(err.response.data);
            });
    }

    getUserMenus() {
        let headers = {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + Auth.getToken(),
            }
        };

        axios.get('/api/v1/dishes/owner', headers)
            .then(response => {
                console.log(response);
                this.setState({ userDishes: response.data, loading: false });
            }).catch(err => {
                console.log(err.response.data);
                alert(err.response.data);
            });
    }

    getUserMenus() {
        let headers = {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + Auth.getToken(),
            }
        };

        axios.get('/api/v1/menus/owner', headers)
            .then(response => {
                console.log(response);
                this.setState({ userMenus: response.data, loading: false });
            }).catch(err => {
                console.log(err.response.data);
                alert(err.response.data);
            });
    }

    getUserRestaurants() {
        let headers = {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + Auth.getToken(),
            }
        };

        axios.get('/api/v1/restaurants/owner', headers)
            .then(response => {
                console.log(response);
                this.setState({
                    userRestaurants: response.data,
                    loading: false,
                    restaurantId: response.data[0].restaurantId
                });

            }).catch(err => {
                console.log(err.response.data);
                alert(err.response.data);
            });
    }

    handleChange(e) {
        console.log(e);
        this.setState(
            {
                [e.target.id]: e.target.value
            }
        )
    }


    static renderUserDishist(dishes) {
        return <div className='menusList' >
            {dishes.map(dish =>
                <Dish dish={dish} />
            )}
        </div>
    }
}
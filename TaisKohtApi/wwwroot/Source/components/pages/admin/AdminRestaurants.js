import * as React from 'react';
import 'es6-promise';
import 'isomorphic-fetch';
import { Button, FormGroup, FormControl, ControlLabel } from "react-bootstrap";
import AuthService from '../Auth/AuthService';
import RestaurantListItem from './RestaurantListItem';
const Auth = new AuthService();
import './admin.css';

export default class AdminRestaurants extends React.Component {
    constructor() {
        super();
        this.state = {
            restaurants: [],
            loading: true,
            name: null,
            url: null,
            addressFirstLine: null,
            locality: null,
            postcode: null,
            region: null,
            country: null
        };

        this.handleChange = this.handleChange.bind(this);
        this.postRestaurant = this.postRestaurant.bind(this);

        this.getUserRestaurants();
    }
    render() {
        let contents = this.state.loading ? <p><em>Loading...</em></p>
            : AdminRestaurants.renderUserRestaurantList(this.state.restaurants);

        return <div>
            <div className="page-header">Profile</div>
            Your restaurants: 
            {contents}

            <div className="EditForm">
                <div className="page-header">Add new Restaurant</div>
                <form onSubmit={this.postRestaurant}>
                    <FormGroup controlId="name" bsSize="small">
                        <ControlLabel>Restaurant name</ControlLabel>
                        <FormControl
                            autoFocus
                            type="text"
                            value={this.state.name}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="url" bsSize="small">
                        <ControlLabel>Website</ControlLabel>
                        <FormControl
                            autoFocus
                            type="text"
                            value={this.state.url}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="email" bsSize="small">
                        <ControlLabel>Company email</ControlLabel>
                        <FormControl
                            autoFocus
                            type="email"
                            value={this.state.email}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="addressFirstLine" bsSize="small">
                        <ControlLabel>Address</ControlLabel>
                        <FormControl
                            autoFocus
                            type="text"
                            value={this.state.addressFirstLine}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="locality" bsSize="small">
                        <ControlLabel>Locality</ControlLabel>
                        <FormControl
                            autoFocus
                            type="text"
                            value={this.state.locality}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="postcode" bsSize="small">
                        <ControlLabel>Postcode</ControlLabel>
                        <FormControl
                            autoFocus
                            type="text"
                            value={this.state.postcode}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="region" bsSize="small">
                        <ControlLabel>Region</ControlLabel>
                        <FormControl
                            autoFocus
                            type="text"
                            value={this.state.region}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="country" bsSize="small">
                        <ControlLabel>Country</ControlLabel>
                        <FormControl
                            autoFocus
                            type="text"
                            value={this.state.country}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <Button block bsSize="large" type="submit">Add Restaurant</Button>
                   
                </form>
            </div>
        </div>;
    }

    postRestaurant(e) {
        e.preventDefault();
        var postData = {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + Auth.getToken(),
            },
            body: JSON.stringify({
                'name': this.state.name,
                'url': this.state.url,
                'address': {
                    "addressFirstLine": this.state.addressFirstLine,
                    "locality": this.state.locality,
                    "postcode": this.state.postcode,
                    "region": this.state.region,
                    "country": this.state.country
                }
            })
        };

        fetch('/api/v1/Restaurants', postData)
            .then(response => response.json())
            .then(data => {
                console.log(data);
                this.getUserRestaurants();
        });
    }

    getUserRestaurants() {
        fetch('/api/v1/Restaurants', {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + Auth.getToken(),
            }
        }).then(response => response.json())
            .then(data => {
                console.log(data);
                this.setState({ restaurants: data, loading: false });
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

    static renderUserRestaurantList(restaurants) {
        return <div className='restaurantList'>
            {restaurants.map(restaurant =>
                <RestaurantListItem restaurant={restaurant} />
            )}
        </div>;
    }
}
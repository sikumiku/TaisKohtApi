import * as React from 'react';
import 'es6-promise';
import 'isomorphic-fetch';
import AuthService from '../Auth/AuthService';
const Auth = new AuthService();

export default class Profile extends React.Component {
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

        //this.getUserRestaurants();
    }
    render() {

        let contents =  Profile.renderUserRestaurantList(this.state.restaurants);

        return <div>
            <div className="page-header">Profile</div>
            Your restaurants: 
            {contents}

            <div className="card">
                <h1>Add new restaurant</h1>
                <form onSubmit={this.postRestaurant}>
                    <input
                        className="form-item"
                        placeholder="Restaurant name"
                        name="name"
                        type="text"
                        onChange={this.handleChange}
                    />
                    <input
                        className="form-item"
                        placeholder="Url goes here"
                        name="url"
                        type="text"
                        onChange={this.handleChange}
                    />
                    <input
                        className="form-item"
                        placeholder="Email goes here"
                        name="email"
                        type="text"
                        onChange={this.handleChange}
                    />
                    <input
                        className="form-item"
                        placeholder="Address first line goes here"
                        name="addressFirstLine"
                        type="text"
                        onChange={this.handleChange}
                    />
                    <input
                        className="form-item"
                        placeholder="Locality goes here"
                        name="locality"
                        type="text"
                        onChange={this.handleChange}
                    />
                    <input
                        className="form-item"
                        placeholder="Postcode goes here"
                        name="postcode"
                        type="text"
                        onChange={this.handleChange}
                    />
                    <input
                        className="form-item"
                        placeholder="Region goes here"
                        name="region"
                        type="text"
                        onChange={this.handleChange}
                    />
                    <input
                        className="form-item"
                        placeholder="Country goes here"
                        name="country"
                        type="text"
                        onChange={this.handleChange}
                    />
                    <input
                        className="form-submit"
                        value="SUBMIT"
                        type="submit"
                    />
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

        fetch('api/v1/Restaurants', postData)
            .then(response => response.json())
            .then(data => {
                console.log(data);
        });
    }

    handleChange(e) {
        console.log(e);
        this.setState(
            {
                [e.target.name]: e.target.value
            }
        )
    }

    static renderUserRestaurantList(restaurants) {
        return <div className='restaurantList'>
            your restaurant list here with edit and delete buttons.
            </div>
    }
}
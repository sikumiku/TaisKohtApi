import * as React from 'react';
import 'es6-promise';
import 'isomorphic-fetch';
import AuthService from '../Auth/AuthService';
const Auth = new AuthService();

export default class AdminMenus extends React.Component {
    constructor() {
        super();
        this.state = {
            menus: [],
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
        this.postRestaurant = this.postMenu.bind(this);

        this.getUserMenus();
    }
    render() {

        let contents = AdminMenus.renderUserMenuList(this.state.menus);

        return <div>
            <div className="page-header">Manage menus</div>
            Your menus: 
            {contents}

            <div className="card">
                <h1>Add new menu</h1>
                <form onSubmit={this.postMenu}>
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

    postMenu(e) {
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

        fetch('api/v1/menu', postData)
            .then(response => response.json())
            .then(data => {
                console.log(data);
        });
    }

    getUserMenus() {
        fetch('api/v1/menus')
            .then(response => response.json())
            .then(data => {
                console.log(data);
                this.setState({ menus: data, loading: false });
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

    static renderUserMenuList(restaurants) {
        return <div className='restaurantList'>
            your restaurant list here with edit and delete buttons.
            </div>
    }
}
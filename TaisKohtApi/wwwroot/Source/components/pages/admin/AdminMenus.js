import * as React from 'react';
import 'es6-promise';
import 'isomorphic-fetch';
import { Button, FormGroup, FormControl, ControlLabel } from "react-bootstrap";
import AuthService from '../Auth/AuthService';
import RestaurantListItem from './RestaurantListItem';
import MenuListItem from './MenuListItem';
const Auth = new AuthService();
import './admin.css';
import axios from 'axios';


export default class AdminMenus extends React.Component {
    constructor() {
        super();

        this.state = {
            menus: [],
            userRestaurants: [],
            loading: true,
            name: null,
            activeFrom: null,
            activeTo: null,
            RepetitionInterval: null,
            restaurantId: null,
        };

        this.handleChange = this.handleChange.bind(this);
        this.postMenu = this.postMenu.bind(this);

        this.getUserMenus();
        this.getUserRestaurants();
    }
    render() {
        let contents = this.state.loading ? <p><em>Loading...</em></p>
            : AdminMenus.renderUserMenuList(this.state.menus);
        let restaurantOptionValues =
            this.state.userRestaurants.map(restaurant =>
            <option value={restaurant.restaurantId}>{restaurant.name}</option>
        );

        return <div>
            <div className="page-header">Profile</div>
            Your menus:
            {contents}

            <div className="EditForm">
                <div className="page-header">Add new Menu</div>
                <form onSubmit={this.postMenu}>
                    <FormGroup controlId="name" bsSize="small">
                        <ControlLabel>Menu name</ControlLabel>
                        <FormControl
                            autoFocus
                            type="text"
                            value={this.state.name}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="activeFrom" bsSize="small">
                        <ControlLabel>Active from</ControlLabel>
                        <FormControl
                            autoFocus
                            type="date"
                            value={this.state.activeFrom}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="activeTo" bsSize="small">
                        <ControlLabel>Active to</ControlLabel>
                        <FormControl
                            autoFocus
                            type="date"
                            
                            value={this.state.activeTo}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="RepetitionInterval" bsSize="small">
                        <ControlLabel>Repetition Interval</ControlLabel>
                        <FormControl
                            autoFocus
                            type="number"
                            value={this.state.RepetitionInterval}
                            onChange={this.handleChange}
                        />
                    </FormGroup>
                    <FormGroup controlId="restaurantId" bsSize="small">
                        <ControlLabel>Restaurant</ControlLabel>
                        <FormControl componentClass="select"
                            placeholder="select"
                            value={this.state.restaurantId}
                            onChange={this.handleChange}
                            >
                            {restaurantOptionValues}
                        </FormControl>
                    </FormGroup>
                    <Button block bsSize="large" type="submit">Add Menu</Button>
                </form>
            </div>
        </div>;
    }

    postMenu(e) {
        e.preventDefault();
        let postData = JSON.stringify({
            'name': this.state.name,
            'activeFrom': this.state.activeFrom,
            'activeTo': this.state.activeTo,
            'RepetitionInterval': this.state.repetitionInterval,
            'restaurantId': this.state.restaurantId,
        });

        let headers = {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + Auth.getToken(),
            }
        };

        axios.post('/api/v1/Menus', postData, headers)
            .then(response => {
                console.log(data);
                this.getUserMenus();
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

        axios.get('/api/v1/menus/owner', headers)
            .then(response => {
                console.log(response);
                this.setState({ menus: response.data, loading: false });
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


    static renderUserMenuList(menus) {
        return <div className='menusList' >
            {menus.map(menu =>
                <MenuListItem menu={menu} />
            )}
        </div>
    }
}
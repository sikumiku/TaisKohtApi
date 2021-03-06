﻿import React, { Component } from 'react';
import { BrowserRouter as Router, Route, Link } from 'react-router-dom';

import Header from './components/headerComponent/Header';
import MainPage from './components/pages/mainPage/MainPage';
import RestaurantList from './components/pages/restaurants/RestaurantList';
import LoginForm from './components/pages/auth/Login';
import Logout from './components/pages/auth/Logout';
import AdminRestaurants from './components/pages/admin/AdminRestaurants';
import AdminDishes from './components/pages/admin/AdminDishes';
import AdminMenus from './components/pages/admin/AdminMenus';
import RegisterForm from './components/pages/auth/Register';
import PromotionsList from './components/pages/promotions/PromotionsList';
import RestaurantMenu from './components/pages/menu/RestaurantMenu';

class App extends Component {
    render() {
        return (
            <Router>
                <div className="App">
                    <Header />
                    <Route exact path="/" component={MainPage} />
                    <Route exact path="/promotions" component={PromotionsList} />
                    <Route exact path="/restaurants" component={RestaurantList} />
                    <Route exact path="/login" component={LoginForm} />
                    <Route exact path="/logout" component={Logout} />
                    <Route exact path="/register" component={RegisterForm} />
                    <Route exact path="/admin/restaurants" component={AdminRestaurants} />
                    <Route exact path="/admin/dishes" component={AdminDishes} />
                    <Route exact path="/admin/menus" component={AdminMenus} />

                </div>
            </Router>
        );
    }
}

export default App;

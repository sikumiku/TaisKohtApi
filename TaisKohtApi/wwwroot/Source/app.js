import React, { Component } from 'react';
import { BrowserRouter as Router, Route, Link } from 'react-router-dom';

import Header from './components/headerComponent/header';
import MainPage from './components/pages/mainPage/MainPage';
import RestaurantList from './components/pages/restaurants/RestaurantList';
import LoginForm from './components/pages/auth/login';
import Logout from './components/pages/auth/logout';
import Profile from './components/pages/profile/profile';

import RegisterForm from './components/pages/auth/register';
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
                    <Route exact path="/profile" component={Profile} />
                </div>
            </Router>
        );
    }
}

export default App;

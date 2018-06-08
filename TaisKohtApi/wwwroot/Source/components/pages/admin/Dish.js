import * as React from 'react';
import 'es6-promise';
import { Button } from "react-bootstrap";
import AuthService from '../Auth/AuthService';
const Auth = new AuthService();
import axios from 'axios';

export default class Dish extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            dish: this.props.dish
        };

        this.deleteDish = this.deleteDish.bind(this);
    }
    render() {
        let contents;
        if (this.state.dish !== null) {
            contents = <div>
                <b> {this.state.dish.title} {this.state.dish.price} </b> <Button bsStyle="link" onClick={() => { this.deleteDish(this.state.dish.dishId) }}>X</Button>
            </div>;
         } else {
            contents = "";
        }
        
        return (
            <div>
                {contents}
            </div>
        );
    }

    deleteDish(dishId) {
        console.log('deleteDish : ' + dishId);
        let headers = {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + Auth.getToken(),
            }
        };

        axios.delete('/api/v1/dishes/'+ dishId, headers)
            .then(response => {
                console.log(response.date);
                this.setState({
                    dish:null
                });

            }).catch(err => {
                console.log(err.response.data);
                alert(err.response.data);
            });
    }

   
}
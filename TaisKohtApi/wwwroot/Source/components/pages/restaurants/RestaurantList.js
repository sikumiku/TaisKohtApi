import * as React from 'react';
import 'es6-promise';
import 'isomorphic-fetch';
import RestoListItem from './restoListItem';
export default class RestaurantList extends React.Component {
    constructor() {
        super();
        this.state = { restos: [], loading: true };
        fetch('api/v1/Restaurants')
            .then(response => response.json())
            .then(data => {
                this.setState({ restos: data, loading: false });
            });
    }
    render() {
        let contents = this.state.loading ? <p><em>Loading...</em></p>
            : RestaurantList.renderRestoTable(this.state.restos);

        return <div>
            <h1>Resto List</h1>
            <button onClick={() => { this.refreshData() }}>Refresh</button>
            <p>This component fetches resto data from the server.</p>
            {contents}
        </div>;
    }
    refreshData() {
        fetch('api/v1/Restaurants')
            .then(response => response.json())
            .then(data => {
                this.setState({ restos: data, loading: false });
            });
    }
    static renderRestoTable(restos) {
        return <div className='restoList'>
            {restos.map(resto =>
                <RestoListItem resto={resto} />
            )}
        </div>;
    }
}
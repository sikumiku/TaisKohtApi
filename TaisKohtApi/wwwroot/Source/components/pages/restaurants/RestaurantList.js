import * as React from 'react';
import 'es6-promise';
import 'isomorphic-fetch';
export default class RestaurantList extends React.Component {
    constructor() {
        super();
        this.state = { restos: [], loading: true };
        fetch('api/Resto/List')
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
        fetch('api/Resto/List')
            .then(response => response.json())
            .then(data => {
                this.setState({ restos: data, loading: false });
            });
    }
    static renderRestoTable(restos) {
        return <table className='table'>
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Address</th>
                    <th>Rating</th>
                </tr>
            </thead>
            <tbody>
                {restos.map(resto =>
                    <tr key={resto.name}>
                        <td>{resto.name}</td>
                        <td>{resto.address}</td>
                        <td>{resto.rating}</td>
                    </tr>
                )}
            </tbody>
        </table>;
    }
}
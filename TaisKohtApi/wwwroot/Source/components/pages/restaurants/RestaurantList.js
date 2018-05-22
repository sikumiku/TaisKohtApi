import * as React from 'react';
import 'es6-promise';
import 'isomorphic-fetch';
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
        return <table className='table'>
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Url</th>
                    <th>Contactnumber</th>
                    <th>Email</th>
                </tr>
            </thead>
            <tbody>
                {restos.map(resto =>
                    <tr key={resto.name}>
                        <td>{resto.name}</td>
                        <td>{resto.url}</td>
                        <td>{resto.contactNumber}</td>
                        <td>{resto.email}</td>
                    </tr>
                )}
            </tbody>
        </table>;
    }
}
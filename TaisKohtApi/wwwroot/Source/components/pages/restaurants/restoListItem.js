import React, { Component } from 'react';

class RestoListItem extends Component {
    render() {
        const resto = this.props.resto;
        return (
            <div>
                {resto.name}
                {resto.url}
                {resto.contactNumber}
                {resto.email}
    	    </div>
        );
    }
}

export default RestoListItem;



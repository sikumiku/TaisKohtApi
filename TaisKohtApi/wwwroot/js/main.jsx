var ContentBox = React.createClass({
    render: function () {
        return (
            <div className="contentBox">
              Main
      </div>
        );
    }
});
ReactDOM.render(
    <ContentBox />,
    document.getElementById('content')
);
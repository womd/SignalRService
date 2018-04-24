var MinerListBox = React.createClass({


    getInitialState: function () {

        window.setTimeout(function () {

            servicehub.server.getMinerlistInitialState().done(function (data) {

                this.setState({ data: data });
            }).fail(function () {

                alert("failed");

                });
            

        }, 3000);
        
        return null;
    },

    render: function () {
        return (
            React.createElement('div', { className: 'MinerListBox' },
                <MinerList data={this.state.data} />
            )

        );
    }
});


var MinerList = React.createClass({

    render: function () {

        var miners = this.props.data.map(function (miner) {
            return (
                <Miner ConnectionId={miner.ConnectionId} key={miner.Id} ClientIp={miner.ClientIp}></Miner>
               
            );
        });


        return (

            <div className="MinerList">
                {miners}
            </div>
        );

    }
});

var Miner = React.createClass({

    render: function () {
        return (
            <div className="Miner">
                <div className="ConnectionId">
                    {this.props.ConnectionId}
                </div>
                <div className="ClientIp">
                    {this.props.ClientIp}
                </div>
            </div>
        );
    }
});

var data = [
    { Id: 234, ConnectionId: "12312312322", ClientIp: "127.0.0.1" },
    { Id: 444, ConnectionId: "12312312333", ClientIp: "127.0.0.2" }
];


ReactDOM.render(<MinerListBox data={data} />, document.getElementById('content'));
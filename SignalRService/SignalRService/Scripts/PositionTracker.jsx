
window.findReactComponent = function (el) {
    for (const key in el) {
        if (key.startsWith('__reactInternalInstance$')) {
            const fiberNode = el[key];

            return fiberNode && fiberNode.return && fiberNode.return.stateNode;
        }
    }
    return null;
};

class PositionTracker extends React.Component {
    constructor(props) {
        super(props);
        this.state = { };
    }
    componentDidMount() {
       
    }

    render() {

        return (
            <div>
               -- -- --
            </div>);
    }
}


//



// ========================================
function startPositionTracker() {

     gameElement =  ReactDOM.render(
            <PositionTracker />,
            document.getElementById('PositionTracker')
        );
   
}



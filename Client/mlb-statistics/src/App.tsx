import React from 'react';

class PlayerLetters extends React.Component{
  state = {
    results: []
  };

  render(){
    let letters = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z']
    let currentKey = 0;

    return (
      letters.map((value: string) =>
        <div key={currentKey++} className='Button-Link'>

          <button onClick={() =>{
            fetch('https://localhost:7287/Players/' + value.toLowerCase())
              .then(response => response.json())
              .then(data => this.setState({results: data}));
          }}>{value}</button>

          <span>{this.state.results}</span>
        </div>
      )
    );
  }
}

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <PlayerLetters />
      </header>
    </div>
  );
}

export default App;

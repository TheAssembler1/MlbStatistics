import './App.css';
import React from 'react';

class Letter extends React.Component{
  render(){
    return (
      <span className='PlayerLetter'>
        <button onClick={() => this.props.handleClick(this.props.letter)}>{this.props.letter}</button>
      </span>
    );
  }
}

class PlayerLetters extends React.Component{
  state = {
    results: [],
  }

  handleClick = (letter) => {
    let url = 'https://localhost:7287/Players/' + letter.toLowerCase();

    console.log(url);

    fetch(url)
      .then(response => response.json())
      .then(data => this.setState({results: data}))
  }

  render(){
    const alphabet = ["A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"];

    
    return (
      <div>
        {alphabet.map((letter) => (
          <Letter key={letter} letter={letter} handleClick={this.handleClick} />
        ))}

        {this.state.results.map((name) => (
          <p>{name}</p>
        ))}
      </div>
    );
  }
}

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <h1>Baseball Statistics</h1>
        <p>Made By: TheAssembler1</p>
        <PlayerLetters/>
      </header>
    </div>
  );
}

export default App;

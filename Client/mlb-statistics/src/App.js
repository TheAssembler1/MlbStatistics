import './App.css';
import React from 'react';

//TODO::put all these into their own files

class Letter extends React.Component{
  render(){
    return (
      <span className='PlayerLetter'>
        <button className='PlayerLetterButton' onClick={() => this.props.handleClick(this.props.letter)}>{this.props.letter}</button>
      </span>
    );
  }
}

class PlayerLetters extends React.Component{
  //FIXME::temp urls just for now
  myApi = 'https://localhost:7287/Players/';
  statReferenceApi = 'https://www.baseball-reference.com/';

  state = {
    results: [],
  }

  handleClick = (letter) => {
    let url = 'https://localhost:7287/Players/NamesUris/' + letter.toLowerCase();

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
          <Letter letter={letter} handleClick={this.handleClick} />
        ))}

        <hr></hr>

        {this.state.results.map((playerNameUri) => (
          <div>
            <a href={this.statReferenceApi + playerNameUri} className='PlayerNameButton'>{playerNameUri._name}</a>
          </div>
        ))}
      </div>
    );
  }
}

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <h1 >Baseball Statistics</h1>
        <h4>Made By: TheAssembler1</h4>
        <PlayerLetters/>
      </header>
    </div>
  );
}

export default App;

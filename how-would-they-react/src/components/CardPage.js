// src/components/CardPage.js
import React, { useState } from 'react';
import ReactCardFlip from 'react-card-flip';

const CardPage = ({ celebrityName, celebrityImage, answer }) => {
  const [isFlipped, setIsFlipped] = useState(false);

  const handleCardClick = () => {
    setIsFlipped(!isFlipped);
  };

  return (
    <div>
      <h2>Card Page</h2>
      <ReactCardFlip isFlipped={isFlipped} flipDirection="horizontal">
        <div key="front" onClick={handleCardClick}>
          <img src={celebrityImage} alt={celebrityName} />
          <p>{celebrityName}</p>
        </div>
        <div key="back" onClick={handleCardClick}>
          <p>{answer}</p>
        </div>
      </ReactCardFlip>
    </div>
  );
};

export default CardPage;

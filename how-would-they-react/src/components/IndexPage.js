// src/components/IndexPage.js
import React, { useState } from 'react';
import QuestionPage from './QuestionPage';
import CardPage from './CardPage';
import Ollama from "ollama";

const IndexPage = () => {
  const [question, setQuestion] = useState('');
  const [answer, setAnswer] = useState(''); // Set the answer from the Llama2 model

  const handleQuestionSubmit = async (userQuestion) => {
    // Here you would invoke the Llama2 model and set the answer
    // For now, let's set a placeholder answer

    setQuestion(userQuestion);

    // const response = await Ollama.chat({
    //   model: 'llama2',
    //   messages: [{ role: 'user', content: question}]
    // });
    
    // console.log(response.message.content);
    // setAnswer(response.message.content);
    setAnswer('This is a placeholder answer.');

    
  };

  return (
    <div>
      <h1>Index Page</h1>
      <QuestionPage onQuestionSubmit={handleQuestionSubmit} />
      <CardPage celebrityName="Celebrity Name" celebrityImage="url/to/image.jpg" answer={answer} />
    </div>
  );
};

export default IndexPage;

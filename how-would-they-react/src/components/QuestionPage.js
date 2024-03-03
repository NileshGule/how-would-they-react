// src/components/QuestionPage.js
import React, { useState } from 'react';

const QuestionPage = ({ onQuestionSubmit }) => {
  const [question, setQuestion] = useState('');

  const handleInputChange = (e) => {
    setQuestion(e.target.value);    
  };

  const handleSubmit = () => {
    alert('Question submitted: ' + question);
    onQuestionSubmit(question);
  };

  return (
    <div>
      <h2>Question Page</h2>
      <textarea
        placeholder="Enter your question here..."
        value={question}
        onChange={handleInputChange}
      />
      <button onClick={handleSubmit}>Submit Question</button>
    </div>
  );
};

export default QuestionPage;

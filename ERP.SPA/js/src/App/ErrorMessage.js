import React from 'react'

const ErrorMessage = (props) => (
  <div className='error-toast'>
      <div className='error-title'>{props.title}</div>
      <div className='error-message'>{props.message}</div>
      <div className='error-additional'>{props.additional}</div>
  </div>
)
export default ErrorMessage
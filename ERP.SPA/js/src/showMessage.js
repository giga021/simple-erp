import { toast } from 'react-toastify';
import ErrorMessage from './App/ErrorMessage';
import React from 'react'

const showMessage = {
    info: (content) => toast.info(content),
    warning: (content) => toast.warn(content),
    error: (content, title, additional) => 
        toast.error(<ErrorMessage title={title} message={content} additional={additional} />)
}
export default showMessage;
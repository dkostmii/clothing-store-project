import { useState } from 'react'

import "./Password.css"

function Password({ onChange, value }) {
  const [masked, setMasked] = useState(true)

  return (
    <div className="field-container">
      <label>Password</label>
      <div className="password-input">
        <input type={ masked ? "password" : "text" } placeholder="Password" onChange={onChange} value={value} />
        <button onClick={() => setMasked(!masked)}>{ masked ? "Show" : "Hide" }</button>
      </div>
    </div>
  )
}

export default Password

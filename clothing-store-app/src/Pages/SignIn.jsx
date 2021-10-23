import { useState } from 'react'
import { Role } from '../Types/Role'
import { SignInRequest as Request } from '../Services/Request'
import { SignInValidate as Validate, ValidateRole } from '../Validators/CredentialsValidate'

import Password from '../Components/Password'

import './SignIn.css'

const Roles = Object.values(Role)

const emptyState = {
    login: null,
    password: null,
    role: Role.Customer
}

function SignIn({ onSignIn, host }) {
  if (!host) {
    throw new Error("Host prop is not provided")
  }

  const [state, setState] = useState(emptyState)

  const [error, setError] = useState()

  const errorMessage = <p style={{color: '#FF0000'}}>{error && (error.toString ? error.toString() : "Error occurred") }</p>

  const buttonClicked = () => {
    if (!Validate(state)) {
      setError("Provided data is not valid")
      return
    }
    Request(state, json => {
      onSignIn(json)  
    }, error => {
      setError(error)
    }, host)
  }

  const loginChanged = e => {
    setError()
    setState(prevState => {
      return { ...prevState, login: e.target.value }
    })
  }

  const passwordChanged = e => {
    setError()
    setState(prevState => {
      return { ...prevState, password: e.target.value }
    })
  }

  const selectUpdated = e => {
    if (!ValidateRole(e.target.value)) {
      throw new Error("Invalid role!")
    }

    setError()
    setState(prevState => {
      return { ...prevState, role: e.target.value }
    })
  }

  return (
    <div className="signin-form-container">
      <h1>Sign in</h1>
      <div className="field-container">
        <label>Login</label><input type="text" placeholder="Login" onChange={loginChanged} />
      </div>
      <Password onChange={passwordChanged} />
      <div className="field-container">
        <label>Role</label>
        <select onChange={selectUpdated}>
          { Roles.map((role, id) => <option key={id} value={role}>{role}</option>) }
        </select>
      </div>
      <button onClick={buttonClicked}>Sign in</button>
      { error && errorMessage }
    </div>
  )
}

export default SignIn

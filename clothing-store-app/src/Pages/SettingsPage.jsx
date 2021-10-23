import { useState, useEffect } from 'react'

import Password from '../Components/Password'

import './SettingsPage.css'

import { SettingsValidate } from '../Validators/SettingsValidate'

import { SettingsRequest as Request } from '../Services/Request'

function SettingsPage({ authentication, onUserChanged, host }) {
  if (!host) {
    throw new Error("Host prop is not provided")
  }

  const { user, token } = authentication
  const [editing, setEditing] = useState(false)

  const [json, setJson] = useState(user)
  const [data, setData] = useState(user)

  const [info, setInfo] = useState()

  const [error, setError] = useState()
  
  const postSettingsData = () => {
    if (!token) {
      throw new Error("User token is missing")
    }

    Request((user, message) => {
      setJson(user)
      setInfo(message)
      setEditing(false)
      onUserChanged(user)
    }, error => {
      console.log(error)
      setError(error)
    }, host, { id: user.id, token, data })
  }

  const errorMessage = <p style={{color: '#FF0000'}}>Error updating settings: {JSON.stringify(error)}</p>

  const updateData = (data) => {
    setError()
    setData(prevData => {
      return { ...prevData, ...data }
    })
  }

  const firstNameChanged = ({ target }) => {
    updateData({ firstName: target.value })
  }

  const lastNameChanged = ({ target }) => {
    updateData({ lastName: target.value })
  }

  const emailChanged = ({ target }) => {
    updateData({ email: target.value })
  }

  const phoneChanged = ({ target }) => {
    updateData({ phone: target.value })
  }

  const passwordChanged = ({ target }) => {
    updateData({ password: target.value })
  }

  return (
    <div className="settings-page-container">
      <div className="settings-wrapper">
        <div>
          <label>First Name</label>
          { editing ? 
            <input type="text" value={data.firstName} onChange={firstNameChanged} /> : 
            <label>{json.firstName}</label> 
          }
        </div>
        <div>
          <label>Last Name</label>
          { editing ? 
            <input type="text" value={data.lastName} onChange={lastNameChanged} /> : 
            <label>{json.lastName}</label> 
          }
        </div>
        <div>
          <label>Email</label>
          { editing ? 
            <input type="text" value={data.email} onChange={emailChanged} /> : 
            <label>{json.email}</label> 
          }
        </div>
        <div>
          <label>Phone</label>
          { editing ? 
            <input type="text" value={data.phone} onChange={phoneChanged} /> : 
            <label>{json.phone}</label> 
          }
        </div>
        <div>
          <label>Password</label>
          { editing ? 
            <Password onChange={passwordChanged} /> : 
            <label>{ "•".repeat(data.password ? data.password.length : 10) }</label> 
          }
        </div>

        <button className="default-button" 
          onClick={() => {
            if (!editing) {
              setData(json)
              setEditing(true)
            } else {
              if (!SettingsValidate(data)) {
                setError("New settings are not valid")
              } else {
                postSettingsData()
              }
            }
          }}>{ editing ? "Save" : "Edit" }</button>
          { editing && <button className="invert-button" onClick={() => {
            setEditing(false)
            setData(json)
          }}>Cancel</button> } 

        { error && errorMessage }
        { info && <div className="info">{info}</div> }
      </div>
    </div>
  )

}

export default SettingsPage
// process.loadEnvFile() // uncomment in development mode
import { Request, Response } from "express"
import axios from 'axios'
import { genJWT } from "../config/config"

const API_SERVER = process.env.API_SERVER!

export const getAll = async (_req: Request, res: Response) => {
  try {
    const token = await genJWT()
    const reqServer: any  = await axios.get(`http://${API_SERVER}/api/pelicula`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    })

    return res.render('index', { data: reqServer.data })
  } catch (err) {
    console.log(err)
    return res.status(500).json({ error: "an error ocurred" })
  }
}

export const searchTF = async (req: Request, res: Response) => {
  try {
    const token = await genJWT()
    const { text } = req.params
    const reqServer: any  = await axios.get(`http://${API_SERVER}/api/pelicula/search/${text}`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    })

    return res.render('index', { data: reqServer.data })
  } catch (err) {
    console.log(err)
    return res.status(500).json({ error: "an error ocurred" })
  }
}

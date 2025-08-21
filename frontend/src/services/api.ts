import axios from 'axios';
import { Property, PropertyFilter, PropertyCreate, PropertyUpdate, Owner, OwnerCreate, ApiResponse } from '@/types';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5107/api';

console.log('API Base URL:', API_BASE_URL);

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 15000, 
});

apiClient.interceptors.request.use(
  (config) => {
    console.log(`API Request:`, {
      method: config.method?.toUpperCase(),
      url: `${config.baseURL}${config.url}`,
      data: config.data,
      headers: config.headers,
    });
    return config;
  },
  (error) => {
    console.error('API Request Setup Error:', error);
    return Promise.reject(error);
  }
);

apiClient.interceptors.response.use(
  (response) => {
    console.log(`API Response:`, {
      status: response.status,
      statusText: response.statusText,
      url: response.config.url,
      data: response.data,
    });
    return response;
  },
  (error) => {
    console.error('API Error Details:', {
      message: error.message,
      code: error.code,
      status: error.response?.status,
      statusText: error.response?.statusText,
      data: error.response?.data,
      url: error.config?.url,
      baseURL: error.config?.baseURL,
      method: error.config?.method,
      requestData: error.config?.data,
      headers: error.config?.headers,
    });
    
    if (error.code === 'ECONNREFUSED') {
      throw new Error(`No se puede conectar con el servidor en ${API_BASE_URL}. Verifica que la API esté ejecutándose.`);
    }
    
    if (error.code === 'NETWORK_ERROR' || error.message === 'Network Error') {
      throw new Error(`Error de red. Verifica que el backend esté corriendo en ${API_BASE_URL}`);
    }
    
    if (error.response?.status === 400) {
      const errorMessage = error.response?.data?.message || 'Datos inválidos';
      throw new Error(`Error de validación: ${errorMessage}`);
    }
    
    if (error.response?.status === 404) {
      throw new Error('Recurso no encontrado');
    }
    
    if (error.response?.status >= 500) {
      const errorMessage = error.response?.data?.message || 'Error interno del servidor';
      throw new Error(`Error del servidor: ${errorMessage}`);
    }
    
    if (error.response?.status === 0) {
      throw new Error(`No se puede conectar con ${API_BASE_URL}. Verifica CORS y que el servidor esté corriendo.`);
    }
    
    // Error genérico
    const errorMessage = error.response?.data?.message || error.message || 'Error desconocido';
    throw new Error(errorMessage);
  }
);

// Property API
export const propertyApi = {  
  getAll: async (): Promise<ApiResponse<Property[]>> => {
    try {
      const response = await apiClient.get('/properties');
      return response.data;
    } catch (error) {
      console.error('Error in getAll:', error);
      throw error;
    }
  },

  // Obtener propiedad por ID
  getById: async (id: string): Promise<ApiResponse<Property>> => {
    try {
      const response = await apiClient.get(`/properties/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error in getById:', error);
      throw error;
    }
  },

  // Buscar propiedades con filtros
  search: async (filters: PropertyFilter): Promise<ApiResponse<Property[]>> => {
    try {
      const params = new URLSearchParams();
      
      if (filters.name) params.append('name', filters.name);
      if (filters.address) params.append('address', filters.address);
      if (filters.minPrice !== undefined && filters.minPrice > 0) {
        params.append('minPrice', filters.minPrice.toString());
      }
      if (filters.maxPrice !== undefined && filters.maxPrice > 0) {
        params.append('maxPrice', filters.maxPrice.toString());
      }
      if (filters.page) params.append('page', filters.page.toString());
      if (filters.pageSize) params.append('pageSize', filters.pageSize.toString());

      const queryString = params.toString();
      const url = queryString ? `/properties/search?${queryString}` : '/properties/search';
      
      console.log('🔍 Search URL:', `${API_BASE_URL}${url}`);
      
      const response = await apiClient.get(url);
      return response.data;
    } catch (error) {
      console.error('Error in search:', error);
      throw error;
    }
  },

  // Crear nueva propiedad
  create: async (property: PropertyCreate): Promise<ApiResponse<Property>> => {
    try {
      console.log('Creating property with data:', property);
      
      if (!property.name?.trim()) {
        throw new Error('El nombre es requerido');
      }
      if (!property.address?.trim()) {
        throw new Error('La dirección es requerida');
      }
      if (!property.idOwner?.trim()) {
        throw new Error('El propietario es requerido');
      }
      if (property.price <= 0) {
        throw new Error('El precio debe ser mayor a 0');
      }
      
      const response = await apiClient.post('/properties', property);
      console.log('Property created successfully:', response.data);
      return response.data;
    } catch (error) {
      console.error('Error creating property:', error);
      throw error;
    }
  },

  // Actualizar propiedad
  update: async (id: string, property: PropertyUpdate): Promise<ApiResponse<Property>> => {
    try {
      const response = await apiClient.put(`/properties/${id}`, property);
      return response.data;
    } catch (error) {
      console.error('Error in update:', error);
      throw error;
    }
  },

  // Eliminar propiedad
  delete: async (id: string): Promise<ApiResponse<boolean>> => {
    try {
      const response = await apiClient.delete(`/properties/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error in delete:', error);
      throw error;
    }
  },
};

// Owner API
export const ownerApi = {
  
  getAll: async (): Promise<ApiResponse<Owner[]>> => {
    try {
      const response = await apiClient.get('/owners');
      return response.data;
    } catch (error) {
      console.error('Error getting owners:', error);
      throw error;
    }
  },

  // Obtener propietario por ID
  getById: async (id: string): Promise<ApiResponse<Owner>> => {
    try {
      const response = await apiClient.get(`/owners/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error getting owner by ID:', error);
      throw error;
    }
  },

  // Crear nuevo propietario
  create: async (owner: OwnerCreate): Promise<ApiResponse<Owner>> => {
    try {
      console.log('Creating owner with data:', owner);
      
      // Validar datos antes de enviar
      if (!owner.name?.trim()) {
        throw new Error('El nombre es requerido');
      }
      if (!owner.address?.trim()) {
        throw new Error('La dirección es requerida');
      }
      if (!owner.phone?.trim()) {
        throw new Error('El teléfono es requerido');
      }
      if (!owner.birthday) {
        throw new Error('La fecha de nacimiento es requerida');
      }
      
      const response = await apiClient.post('/owners', owner);
      console.log('Owner created successfully:', response.data);
      return response.data;
    } catch (error) {
      console.error('Error creating owner:', error);
      throw error;
    }
  },

  // Actualizar propietario
  update: async (id: string, owner: Partial<Owner>): Promise<ApiResponse<Owner>> => {
    try {
      const response = await apiClient.put(`/owners/${id}`, owner);
      return response.data;
    } catch (error) {
      console.error('Error updating owner:', error);
      throw error;
    }
  },

  // Eliminar propietario
  delete: async (id: string): Promise<ApiResponse<boolean>> => {
    try {
      const response = await apiClient.delete(`/owners/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error deleting owner:', error);
      throw error;
    }
  },
};

// Utility functions
export const formatPrice = (price: number): string => {
  return new Intl.NumberFormat('es-CO', {
    style: 'currency',
    currency: 'COP',
    minimumFractionDigits: 0,
    maximumFractionDigits: 0,
  }).format(price);
};

export const formatDate = (dateString: string): string => {
  return new Date(dateString).toLocaleDateString('es-CO', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });
};

export const formatNumber = (num: number): string => {
  return new Intl.NumberFormat('es-CO').format(num);
};
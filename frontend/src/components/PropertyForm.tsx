'use client';

import { useState, useEffect } from 'react';
import { PropertyCreate, Owner, ApiResponse } from '@/types';
import { propertyApi, ownerApi } from '@/services/api';
import { X, Save, User, Home } from 'lucide-react';

interface PropertyFormProps {
  onClose: () => void;
  onSuccess: () => void;
}

export function PropertyForm({ onClose, onSuccess }: PropertyFormProps) {
  const [loading, setLoading] = useState(false);
  const [owners, setOwners] = useState<Owner[]>([]);
  const [formData, setFormData] = useState<PropertyCreate>({
    name: '',
    address: '',
    price: 0,
    idOwner: '',
    image: '',
    year: new Date().getFullYear(),
  });
  const [errors, setErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    loadOwners();
  }, []);

  const loadOwners = async () => {
    try {
      const response = await ownerApi.getAll();
      if (response.success && response.data) {
        setOwners(response.data);
      }
    } catch (error) {
      console.error('Error loading owners:', error);
    }
  };

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {};

    if (!formData.name.trim()) {
      newErrors.name = 'El nombre es requerido';
    }
    if (!formData.address.trim()) {
      newErrors.address = 'La dirección es requerida';
    }
    if (formData.price <= 0) {
      newErrors.price = 'El precio debe ser mayor a 0';
    }
    if (!formData.idOwner) {
      newErrors.idOwner = 'Debe seleccionar un propietario';
    }
    if (formData.year < 1900 || formData.year > new Date().getFullYear() + 5) {
      newErrors.year = 'Año inválido';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    console.log(' Form submitted with data:', formData);
    
    if (!validateForm()) {
      console.log(' Form validation failed');
      return;
    }

    setLoading(true);
    try {
      console.log('Sending request to create property...');
      const response = await propertyApi.create(formData);
      console.log('Property creation response:', response);
      
      if (response.success) {
        console.log('Property created successfully');
        onSuccess();
        onClose();
      } else {
        console.log('Property creation failed:', response.message);
        setErrors({ submit: response.message || 'Error al crear la propiedad' });
      }
    } catch (error) {
      console.error('Error in handleSubmit:', error);
      const errorMessage = error instanceof Error ? error.message : 'Error al crear la propiedad';
      setErrors({ submit: errorMessage });
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (field: keyof PropertyCreate, value: string | number) => {
    setFormData(prev => ({ ...prev, [field]: value }));
    if (errors[field]) {
      setErrors(prev => ({ ...prev, [field]: '' }));
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50 animate-fade-in">
      <div className="bg-white rounded-lg max-w-2xl w-full max-h-[90vh] overflow-y-auto animate-slide-up">
        <div className="flex items-center justify-between p-6 border-b">
          <h2 className="text-xl font-semibold text-gray-900 flex items-center gap-2">
            <Home className="h-5 w-5" />
            Nueva Propiedad
          </h2>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-gray-600 transition-colors"
          >
            <X className="h-6 w-6" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-6 space-y-6">
          {errors.submit && (
            <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded">
              {errors.submit}
            </div>
          )}

          <div className="grid md:grid-cols-2 gap-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Nombre de la Propiedad *
              </label>
              <input
                type="text"
                value={formData.name}
                onChange={(e) => handleChange('name', e.target.value)}
                className={`w-full px-3 py-2 border rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent ${
                  errors.name ? 'border-red-300' : 'border-gray-300'
                }`}
                placeholder="Ej: Apartamento Moderno Centro"
              />
              {errors.name && <p className="text-red-500 text-sm mt-1">{errors.name}</p>}
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Propietario *
              </label>
              <select
                value={formData.idOwner}
                onChange={(e) => handleChange('idOwner', e.target.value)}
                className={`w-full px-3 py-2 border rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent ${
                  errors.idOwner ? 'border-red-300' : 'border-gray-300'
                }`}
              >
                <option value="">Seleccionar propietario</option>
                {owners.map((owner) => (
                  <option key={owner.id} value={owner.id}>
                    {owner.name}
                  </option>
                ))}
              </select>
              {errors.idOwner && <p className="text-red-500 text-sm mt-1">{errors.idOwner}</p>}
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Dirección *
            </label>
            <input
              type="text"
              value={formData.address}
              onChange={(e) => handleChange('address', e.target.value)}
              className={`w-full px-3 py-2 border rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent ${
                errors.address ? 'border-red-300' : 'border-gray-300'
              }`}
              placeholder="Ej: Carrera 15 #85-40, Chapinero, Bogotá"
            />
            {errors.address && <p className="text-red-500 text-sm mt-1">{errors.address}</p>}
          </div>

          <div className="grid md:grid-cols-2 gap-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Precio (COP) *
              </label>
              <input
                type="number"
                value={formData.price}
                onChange={(e) => handleChange('price', Number(e.target.value))}
                className={`w-full px-3 py-2 border rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent ${
                  errors.price ? 'border-red-300' : 'border-gray-300'
                }`}
                placeholder="450000000"
                min="0"
              />
              {errors.price && <p className="text-red-500 text-sm mt-1">{errors.price}</p>}
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Año de Construcción *
              </label>
              <input
                type="number"
                value={formData.year}
                onChange={(e) => handleChange('year', Number(e.target.value))}
                className={`w-full px-3 py-2 border rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent ${
                  errors.year ? 'border-red-300' : 'border-gray-300'
                }`}
                min="1900"
                max={new Date().getFullYear() + 5}
              />
              {errors.year && <p className="text-red-500 text-sm mt-1">{errors.year}</p>}
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              URL de Imagen
            </label>
            <input
              type="url"
              value={formData.image}
              onChange={(e) => handleChange('image', e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="https://images.unsplash.com/photo-example..."
            />
            <p className="text-sm text-gray-500 mt-1">
              Puedes usar imágenes de Unsplash o cualquier URL pública
            </p>
          </div>

          <div className="flex justify-end gap-3 pt-6 border-t">
            <button
              type="button"
              onClick={onClose}
              className="px-6 py-2 border border-gray-300 text-gray-700 rounded-md hover:bg-gray-50 transition-colors"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={loading}
              className="px-6 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-2"
            >
              <Save className="h-4 w-4" />
              {loading ? 'Guardando...' : 'Crear Propiedad'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
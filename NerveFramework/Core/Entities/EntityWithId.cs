﻿using System;

namespace NerveFramework.Core.Entities
{
    /// <summary>
    /// A single unit of relational data that can be identified by a primary key.
    /// </summary>
    /// <typeparam name="TId">The type of the entity's primary key.</typeparam>
    public abstract class EntityWithId<TId> : Entity, IEquatable<EntityWithId<TId>>
    {
        /// <summary>
        /// Primary key of this entity.
        /// </summary>
        public TId Id { get; protected set; }

        /// <summary>
        /// Get the hash code for this entity instance based on its Id property.
        /// </summary>
        /// <returns>The hash code for this object instance based on its Id property.</returns>
        public override int GetHashCode()
        {
            return IsTransient(this) ? 0 : Id.GetHashCode();
        }

        /// <summary>
        /// Determine whether this entity is equal to another object.
        /// </summary>
        /// <param name="other">The object to compare to this entity when determining equality.</param>
        /// <returns>True if the other object is an EntityWithId, both entities are not null or
        /// transient, and both entities have the same Id value. Otherwise, false.</returns>
        public override bool Equals(object other)
        {
            return Equals(other as EntityWithId<TId>);
        }

        /// <summary>
        /// Determine whether this entity is equal to another entity.
        /// </summary>
        /// <param name="other">The entity to compare to this entity when determining equality.</param>
        /// <returns>True if the other entity is not null, neither entity is transient, and both
        /// entities share the same Id value.</returns>
        public virtual bool Equals(EntityWithId<TId> other)
        {
            // instance is never equal to null
            if (other == null) return false;

            // when references are equal, they are the same object
            if (ReferenceEquals(this, other)) return true;

            // when either object is transient or the id's are not equal, return false
            if (IsTransient(this) || IsTransient(other) || !Equals(Id, other.Id)) return false;

            // when the id's are equal and neither object is transient
            // return true when one can be cast to the other
            // because this entity could be generated by a proxy
            var otherType = other.GetUnproxiedType();
            var thisType = GetUnproxiedType();
            return thisType.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(thisType);
        }

        private static bool IsTransient(EntityWithId<TId> obj)
        {
            // an object is transient when its id is the default (null for strings or 0 for numbers)
            return Equals(obj.Id, default(TId));
        }

        private Type GetUnproxiedType()
        {
            return GetType(); // return the unproxied type of the object
        }

    }

}

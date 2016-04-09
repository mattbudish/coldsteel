﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coldsteel.Tests.Doubles;

namespace Coldsteel.Tests
{
    [TestClass]
    public class GameObjectTests
    {
        #region Composite Functionality Tests

        [TestMethod]
        public void CanSetParentGameObject()
        {
            var parent = new GameObject();
            var child = new GameObject();
            child.SetParent(parent);
            Assert.AreSame(child.Parent, parent);
        }

        [TestMethod]
        public void CanAddChildGameObject()
        {
            var parent = new GameObject();
            var child = new GameObject();
            parent.AddChild(child);
            var hasDescendant = parent.IsAncestorOf(child);
            Assert.IsTrue(hasDescendant);
            Assert.AreSame(child.Parent, parent);
        }

        [TestMethod]
        public void MayChainAddChildGameObject()
        {
            var parent = new GameObject();
            var child = new GameObject();
            var child2 = new GameObject();
            Assert.AreSame(parent.AddChild(child)
                .AddChild(child2), parent);
        }

        [TestMethod]
        public void CanRemoveChildGameObject()
        {
            var parent = new GameObject();
            var child = new GameObject();
            parent.AddChild(child);
            parent.RemoveChild(child);
            var hasDescendant = parent.IsAncestorOf(child);
            Assert.IsFalse(hasDescendant);
            Assert.IsNull(child.Parent);
        }

        [TestMethod]
        public void IsAncestorOfIsRecursive()
        {
            var parent = new GameObject();
            var child = new GameObject();
            parent.AddChild(child);
            var grandChild = new GameObject();
            child.AddChild(grandChild);
            Assert.IsTrue(parent.IsAncestorOf(grandChild));
        }

        [TestMethod]
        public void IsDescendantOfIsRecursive()
        {
            var child = new GameObject();
            var parent = new GameObject();
            child.SetParent(parent);
            Assert.IsTrue(child.IsDescendantOf(parent));
            var grandParent = new GameObject();
            parent.SetParent(grandParent);
            Assert.IsTrue(child.IsDescendantOf(grandParent));

        }

        [TestMethod]
        public void WhenParentIsSetObjectIsAddedAsChild()
        {
            var parent = new GameObject();
            var child = new GameObject();
            child.SetParent(parent);
            Assert.IsTrue(parent.IsAncestorOf(child));
        }


        [TestMethod]
        public void WhenAChildIsProvidedANewParentTheChildIsRemovedFromTheOldParent()
        {
            var oldParent = new GameObject();
            var child = new GameObject();
            oldParent.AddChild(child);
            var newParent = new GameObject();
            child.SetParent(newParent);
            Assert.IsFalse(oldParent.IsAncestorOf(child));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotBeAChildOfItself()
        {
            var gameObject = new GameObject();
            gameObject.AddChild(gameObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotAddAsChildIfTargetIsDescendantOfChild()
        {
            var parent = new GameObject();
            var child = new GameObject();
            child.AddChild(parent);
            parent.AddChild(child);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotBeAParentOfItself()
        {
            var parent = new GameObject();
            var child = new GameObject();
            child.AddChild(parent);
            child.SetParent(parent);
        }

        #endregion

        #region Component Management Tests

        [TestMethod]
        public void CanAddGameComponents()
        {
            var gameObject = new GameObject();
            var mockComponent = new MockGameObjectComponent();
            gameObject.AddComponent(mockComponent);
            Assert.AreSame(gameObject.GetComponent<MockGameObjectComponent>(), mockComponent);
            var mockComponent2 = new MockGameObjectComponent();
            gameObject.AddComponent(mockComponent2);
            var components = gameObject.GetComponents<MockGameObjectComponent>();
            Assert.IsTrue(components.Contains(mockComponent));
            Assert.IsTrue(components.Contains(mockComponent2));
        }

        [TestMethod]
        public void WhenDoesNotHaveComponentGetComponentReturnsNull()
        {
            var gameObject = new GameObject();
            Assert.IsNull(gameObject.GetComponent<MockGameObjectComponent>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenHasMultipleOfSameComponentGetComponentThrowsException()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent(new MockGameObjectComponent());
            gameObject.AddComponent(new MockGameObjectComponent());
            gameObject.GetComponent<MockGameObjectComponent>();
        }

        [TestMethod]
        public void CanRemoveComponents()
        {
            var gameObject = new GameObject();
            var mock = new MockGameObjectComponent();
            gameObject.AddComponent(mock);
            gameObject.RemoveComponent(mock);
            Assert.IsNull(gameObject.GetComponent<MockGameObjectComponent>());
        }

        [TestMethod]
        public void AddComponentMayBeChained()
        {
            var gameObject = new GameObject();
            var mock = new MockGameObjectComponent();
            var mock2 = new MockGameObjectComponent();
            Assert.AreSame(gameObject.AddComponent(mock)
                .AddComponent(mock2), gameObject);
        }

        [TestMethod]
        public void ComponentHasGameObjectWhenAdded()
        {
            var gameObject = new GameObject();
            var mock = new MockGameObjectComponent();
            gameObject.AddComponent(mock);
            Assert.AreSame(gameObject, mock.GameObject);
        }

        [TestMethod]
        public void ComponentNoLongerHasGameObjectWhenRemoved()
        {
            var gameObject = new GameObject();
            var mock = new MockGameObjectComponent();
            gameObject.AddComponent(mock);
            gameObject.RemoveComponent(mock);
            Assert.IsNull(mock.GameObject);
        }

        [TestMethod]
        public void UpdatesGameComponents()
        {
            var gameObject = new GameObject();
            var mockComponent = new MockGameObjectComponent();
            gameObject.AddComponent(mockComponent);
            var mockComponent2 = new MockGameObjectComponent();
            gameObject.AddComponent(mockComponent2);
            gameObject.Update(new DummyGameTime());
            Assert.IsTrue(mockComponent.WasUpdated);
            Assert.IsTrue(mockComponent2.WasUpdated);
        }

        [TestMethod]
        public void GameComponentMayBeRemovedDuringUpdate()
        {
            var gameObject = new GameObject();
            var mockComponent = new MockGameObjectComponent();
            gameObject.AddComponent(mockComponent);
            var mockComponent2 = new MockGameObjectComponent();
            mockComponent2.RemoveFromGameObjectDuringUpdate = true;
            gameObject.AddComponent(mockComponent2);
            gameObject.Update(new DummyGameTime());
            // must not throw exception
        }

        #endregion
    }
}